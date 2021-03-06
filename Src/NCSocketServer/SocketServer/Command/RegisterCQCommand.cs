// Copyright (c) 2018 Alachisoft
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using Alachisoft.NCache.Common.Util;
using Alachisoft.NCache.Caching.Queries;
using Alachisoft.NCache.SocketServer.RuntimeLogging;
using System.Diagnostics;
using Alachisoft.NCache.Common.Monitoring;


namespace Alachisoft.NCache.SocketServer.Command
{
    class RegisterCQCommand : CommandBase
    {
        private struct CommandInfo
        {
            public string RequestId;
            public string Query;
            public IDictionary Values;
            public bool notifyAdd;
            public bool notifyUpdate;
            public bool notifyRemove;
            public string clientUniqueId;
            public int addDataFilter;
            public int removeDataFilter;
            public int updateDataFilter;
        }

        private static char Delimitor = '|';

        public override void ExecuteCommand(ClientManager clientManager, Alachisoft.NCache.Common.Protobuf.Command command)
        {
            CommandInfo cmdInfo;
            int overload;
            string exception = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                overload = command.MethodOverload;
                cmdInfo = ParseCommand(command, clientManager);
            }
            catch (Exception exc)
            {
                if (!base.immatureId.Equals("-2"))
                {
                    //PROTOBUF:RESPONSE
                    _serializedResponsePackets.Add(Alachisoft.NCache.Common.Util.ResponseHelper.SerializeExceptionResponse(exc, command.requestID, command.commandID));
                }
                return;
            }

            try
            {
                NCache nCache = clientManager.CmdExecuter as NCache;
                System.Collections.Generic.Dictionary<Runtime.Events.EventType, Runtime.Events.EventDataFilter> arr = new System.Collections.Generic.Dictionary<Runtime.Events.EventType, Runtime.Events.EventDataFilter>();

                QueryDataFilters datafilters = new QueryDataFilters(cmdInfo.addDataFilter, cmdInfo.updateDataFilter, cmdInfo.removeDataFilter);
                string queryId = nCache.Cache.RegisterCQ(cmdInfo.Query, cmdInfo.Values, cmdInfo.clientUniqueId, clientManager.ClientID, cmdInfo.notifyAdd, cmdInfo.notifyUpdate, cmdInfo.notifyRemove, new Alachisoft.NCache.Caching.OperationContext(Alachisoft.NCache.Caching.OperationContextFieldName.OperationType, Alachisoft.NCache.Caching.OperationContextOperationType.CacheOperation), datafilters);
                stopWatch.Stop();
                Alachisoft.NCache.Common.Protobuf.Response response = new Alachisoft.NCache.Common.Protobuf.Response();

                Alachisoft.NCache.Common.Protobuf.RegisterCQResponse registerCQResponse = new Alachisoft.NCache.Common.Protobuf.RegisterCQResponse();
                registerCQResponse.cqId = queryId;
                response.registerCQResponse = registerCQResponse;
                response.requestId = Convert.ToInt64(cmdInfo.RequestId);
                response.commandID = command.commandID;
                response.responseType = Alachisoft.NCache.Common.Protobuf.Response.Type.REGISTER_CQ;

                _serializedResponsePackets.Add(Alachisoft.NCache.Common.Util.ResponseHelper.SerializeResponse(response));
            }
            catch (Exception exc)
            {
                 exception = exc.ToString();
                _serializedResponsePackets.Add(Alachisoft.NCache.Common.Util.ResponseHelper.SerializeExceptionResponse(exc, command.requestID, command.commandID));
            }
            finally
            {
                TimeSpan executionTime = stopWatch.Elapsed;
                try
                {
                    if (Alachisoft.NCache.Management.APILogging.APILogManager.APILogManger != null && Alachisoft.NCache.Management.APILogging.APILogManager.EnableLogging)
                    {
                        APILogItemBuilder log = new APILogItemBuilder(MethodsName.RegisterCQ.ToLower());
                        log.GenerateRegisterCQAPILogItem(cmdInfo.Query, cmdInfo.Values, overload, exception, executionTime, clientManager.ClientID.ToLower(), clientManager.ClientSocketId.ToString());
                    }
                }
                catch
                {
                }
            }
        }

        private CommandInfo ParseCommand(Alachisoft.NCache.Common.Protobuf.Command command, ClientManager clientManager)
        {
            CommandInfo cmdInfo = new CommandInfo();

            Alachisoft.NCache.Common.Protobuf.RegisterCQCommand registerCQCommand = command.registerCQCommand;
            cmdInfo.Query = registerCQCommand.query;
            cmdInfo.RequestId = registerCQCommand.requestId.ToString();
            cmdInfo.notifyAdd = registerCQCommand.notifyAdd;
            cmdInfo.notifyUpdate = registerCQCommand.notifyUpdate;
            cmdInfo.notifyRemove = registerCQCommand.notifyRemove;
            cmdInfo.clientUniqueId = registerCQCommand.clientUniqueId;
            cmdInfo.addDataFilter = registerCQCommand.addDataFilter;
            cmdInfo.removeDataFilter = registerCQCommand.remvoeDataFilter;
            cmdInfo.updateDataFilter = registerCQCommand.updateDataFilter;

            {
                cmdInfo.Values = new Hashtable();
                foreach (Alachisoft.NCache.Common.Protobuf.KeyValue searchValue in registerCQCommand.values)
                {
                    string key = searchValue.key;
                    List<Alachisoft.NCache.Common.Protobuf.ValueWithType> valueWithTypes = searchValue.value;
                    Type type = null;
                    object value = null;

                    foreach (Alachisoft.NCache.Common.Protobuf.ValueWithType valueWithType in valueWithTypes)
                    {
                        string typeStr = valueWithType.type;
                        if (!clientManager.IsDotNetClient)
                        {
                            typeStr = JavaClrTypeMapping.JavaToClr(valueWithType.type);
                        }
                        type = Type.GetType(typeStr, true, true);

                        if (valueWithType.value != null)
                        {
                            try
                            {
                                if (type == typeof(System.DateTime))
                                {
                                    if (clientManager.IsDotNetClient)
                                    {
                                        System.Globalization.CultureInfo enUs = new System.Globalization.CultureInfo("en-US");
                                        value = Convert.ChangeType(valueWithType.value, type, enUs);
                                    }
                                    else
                                    {
                                        // For java client we would be sending ticks instead
                                        // of string representation of Date.
                                        long ticks = long.Parse(valueWithType.value);
                                        value = new DateTime(ticks);
                                    }
                                }
                                else
                                    value = Convert.ChangeType(valueWithType.value, type);
                            }
                            catch (Exception)
                            {
                                throw new System.FormatException("Cannot convert '" + valueWithType.value + "' to " + type.ToString());
                            }
                        }

                        if (!cmdInfo.Values.Contains(key))
                        {
                            cmdInfo.Values.Add(key, value);
                        }
                        else
                        {
                            ArrayList list = cmdInfo.Values[key] as ArrayList; // the value is not array list
                            if (list == null)
                            {
                                list = new ArrayList();
                                list.Add(cmdInfo.Values[key]); // add the already present value in the list
                                cmdInfo.Values.Remove(key); // remove the key from hashtable to avoid key already exists exception
                                list.Add(value);// add the new value in the list
                                cmdInfo.Values.Add(key, list);
                            }
                            else
                            {
                                list.Add(value);
                            }
                        }
                    }
                }
            }

            return cmdInfo;
        }

        private object GetValueObject(string value)
        {
            object retVal = null;

            try
            {
                // Now we move data-type along with the value.So extract them here.
                string[] vals = value.Split(Delimitor);
                object valObj = (object)vals[0];
                string typeStr = vals[1];

                Type objType = System.Type.GetType(typeStr);
                if (objType == typeof(System.DateTime))
                {
                    System.Globalization.CultureInfo enUs = new System.Globalization.CultureInfo("en-US");
                    retVal = Convert.ChangeType(valObj, objType, enUs);
                }
                else
                    retVal = Convert.ChangeType(valObj, objType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
    }
}
