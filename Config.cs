using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Interface;
using Lagrange.Core.Common.Interface.Api;
using Lagrange.Core;

namespace EasyBot
{
    public class Config
    {
        // 这里放一些配置信息
    }

    public class Login
    {
        private BotDeviceInfo deviceInfo = GetDeviceInfo();
        private BotKeystore keyStore = LoadKeystore() ?? new BotKeystore();
        private BotContext bot;

        private Login()
        {
            this.bot = BotFactory.Create(new BotConfig
            {
                UseIPv6Network = false,
                GetOptimumServer = true,
                AutoReconnect = true,
                Protocol = Protocols.Linux
            }, deviceInfo, keyStore);

            // 订阅事件
            bot.Invoker.OnBotOnlineEvent += EasyBot.Settings.OnBotOnlineEvent;
            bot.Invoker.OnBotOfflineEvent += EasyBot.Settings.OnBotOfflineEvent;
            bot.Invoker.OnBotLogEvent += EasyBot.Settings.OnBotLogEvent;
            bot.Invoker.OnBotCaptchaEvent += EasyBot.Settings.OnBotCaptchaEvent;
            bot.Invoker.OnGroupInvitationReceived += EasyBot.Settings.OnGroupInvitationReceived;
            bot.Invoker.OnFriendMessageReceived += EasyBot.Settings.OnFriendMessageReceived;
            bot.Invoker.OnGroupMessageReceived += EasyBot.Settings.OnGroupMessageReceived;
            bot.Invoker.OnTempMessageReceived += EasyBot.Settings.OnTempMessageReceived;
            bot.Invoker.OnGroupAdminChangedEvent += EasyBot.Settings.OnGroupAdminChangedEvent;
            bot.Invoker.OnGroupMemberIncreaseEvent += EasyBot.Settings.OnGroupMemberIncreaseEvent;
            bot.Invoker.OnGroupMemberDecreaseEvent += EasyBot.Settings.OnGroupMemberDecreaseEvent;
            bot.Invoker.OnFriendRequestEvent += EasyBot.Settings.OnFriendRequestEvent;
        }
        public static async Task<Login> Begin()
        {
            var instance = new Login();
            await instance.InitializeAsync(instance.bot);
            return instance;
        }

        private async Task InitializeAsync(BotContext bot)
        {
            // 登陆
            if (!File.Exists("Keystore.json"))
            {
                // 二维码登陆
                var qrCode = await bot.FetchQrCode();
                if (qrCode != null)
                {
                    await File.WriteAllBytesAsync("qrcode.png", qrCode.Value.QrCode);
                    Console.WriteLine("请扫码登陆！二维码路径："+System.IO.Directory.GetCurrentDirectory()+"/qrcode.png");
                    await bot.LoginByQrCode();
                }
            }
            else
            {
                // 密码登陆
                Console.WriteLine("请输入选项(序号)：\n1.直接登陆\n2.清除登陆痕迹后退出(用于登陆异常)");
                while (true)
                {
                    string? tx = Console.ReadLine();
                    if (tx != null)
                    if (tx == "1")
                    {
                        await bot.LoginByPassword();
                        break;
                    }
                    else if (tx == "2")
                    {
                        try
                        {
                            System.IO.File.Delete("Keystore.json");
                            Console.WriteLine("清除完成！按下回车退出");
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                        catch
                        {
                            Console.WriteLine($"删除失败: {tx}\n按下回车退出");
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        Console.WriteLine("请输入正确序号");
                    }
                }
            }
        }

        public static BotDeviceInfo GetDeviceInfo()
        {
            if (File.Exists("DeviceInfo.json"))
            {
                var info = JsonSerializer.Deserialize<BotDeviceInfo>(File.ReadAllText("DeviceInfo.json"));
                if (info != null) return info;

                info = BotDeviceInfo.GenerateInfo();
                File.WriteAllText("DeviceInfo.json", JsonSerializer.Serialize(info));
                return info;
            }
            var deviceInfo = BotDeviceInfo.GenerateInfo();
            File.WriteAllText("DeviceInfo.json", JsonSerializer.Serialize(deviceInfo));
            return deviceInfo;
        }
        public static void SaveKeystore(BotKeystore keystore) =>
        File.WriteAllText("Keystore.json", JsonSerializer.Serialize(keystore));

        public static BotKeystore? LoadKeystore()
        {
            try
            {
                var text = File.ReadAllText("Keystore.json");
                return JsonSerializer.Deserialize<BotKeystore>(text, new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch
            {
                return null;
            }
        }
    }
}
