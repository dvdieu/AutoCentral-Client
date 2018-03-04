using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;

namespace AutoCentral
{
    class ConnectionSocketIO
    {
        private static object syncRoot = new Object();
        private static volatile ConnectionSocketIO instance;
        public Action LoginThanhCong;
        public Action<string> LoginThatBai;

        bool is_connect = false;
        Socket socket;
        
        public bool isConnected{
            get{
                return is_connect;
            }
        }

        public static ConnectionSocketIO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ConnectionSocketIO();
                    }
                }

                return instance;
            }
        }

        public void unRegister()
        {
            is_connect = false;
            if (this.socket != null)
            {
                socket.Disconnect();
            }
        }

        public void connect()
        {
            try
            {
                socket = IO.Socket(ConfigCenter.Instance.getConfig("root-socket"));
                is_connect = true;
            }
            catch (SocketIOException e)
            {
                throw e;
            }
            
        }

        public void register()
        {
            onPlay();
            onStop();
            onDisconect();
            onLogin();
        }
        private ConnectionSocketIO()
        {
            connect();
            register();
        }

        private void onDisconect()
        {
            
        }

        private void onStop()
        {
            try
            {
                socket.On("exit", (data) =>
                {
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.ToString());
                });
            }
            catch (SocketIOException e)
            {
                throw e;
            }
        }

        public void emit(string eventName,string data)
        {
            try
            {
                socket.Emit(eventName, data);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void onLogin()
        {
            socket.On("login-error", (data) =>
            {
                LoginThatBai.Invoke("Tài khoản không tồn tại");
            });
            socket.On("session-is-has", (data) => {
                LoginThatBai.Invoke("Tài khoản đang được sử dụng");
            });
            socket.On("login-success", (data) =>
            {
                LoginThanhCong.Invoke();
            });
        }


        private void onPlay()
        {
            try
            {
                socket.On("server-play-command", (data) =>
                {
                    SocketPlayCommandDTO socketCommandDTO = JsonConvert.DeserializeObject<SocketPlayCommandDTO>(data.ToString());
                    Console.WriteLine(socketCommandDTO.game_id);
                    string tmp = RestSharpWrapper.Instance.get(ConfigCenter.Instance.getConfig("get-command-play") +socketCommandDTO.server+"/"+socketCommandDTO.game_id);
                    DataAPI values = JsonConvert.DeserializeObject<DataAPI>(tmp.ToString());
                    if (values.play)
                    {
                        if (values.type.Equals("T"))
                        {
                            Auto.play(DataAutoIT.title, DataAutoIT.Big.X, DataAutoIT.Big.Y, values.money);
                            //Confirm
                            ConfirmDTO confirmDTO = new ConfirmDTO();
                            confirmDTO.server = DataAutoIT.server;
                            confirmDTO.value = values.money;
                            confirmDTO.account = DataAutoIT.account;
                            confirmDTO.command = "T";
                            confirmDTO.gameid = values.gameid;
                            RestSharpWrapper.Instance.post(ConfigCenter.Instance.getConfig("confirm-play"), confirmDTO);
                        }
                        if (values.type.Equals("X"))
                        {
                            Auto.play(DataAutoIT.title, DataAutoIT.Small.X, DataAutoIT.Small.Y, values.money);
                            ConfirmDTO confirmDTO = new ConfirmDTO();
                            confirmDTO.server = DataAutoIT.server;
                            confirmDTO.value = values.money;
                            confirmDTO.account = DataAutoIT.account;
                            confirmDTO.command = "X";
                            confirmDTO.gameid = values.gameid;
                            RestSharpWrapper.Instance.post(ConfigCenter.Instance.getConfig("confirm-play"),confirmDTO);
                        }
                    }
                });
            }
            catch(SocketIOException e)
            {
                throw e;
            }
        }
    }
}
