using System;
using Lagrange.Core;
using Lagrange.Core.Event.EventArg;

namespace EasyBot
{
    // 在这里修改规则
    public class Settings
    {
        // Bot 上线时触发,可用于监控是否登录成功
        public static void OnBotOnlineEvent(BotContext Bot,BotOnlineEvent @event)
        {
            Console.WriteLine("登陆成功！");
        }

        // Bot 下线时触发,可用于监控 Bot 是否掉线
        public static void OnBotOfflineEvent(BotContext Bot, BotOfflineEvent @event)
        {
            Console.WriteLine("已下线！");
        }

        // 日志产生时触发
        public static void OnBotLogEvent(BotContext Bot, BotLogEvent @event)
        {
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.WriteLine(@event.ToString());
            }
        }

        // Bot 需要验证码时触发
        public static void OnBotCaptchaEvent(BotContext Bot, BotCaptchaEvent @event)
        {
            Console.WriteLine("需要验证码："+@event.Url);
        }

        // Bot 被邀请入群时触发
        public static void OnGroupInvitationReceived(BotContext Bot, GroupInvitationEvent @event)
        {
            Console.WriteLine($"被邀请入群{@event.GroupUin.ToString()}, 邀请人{@event.InvitorUin.ToString()}");
        }

        // 收到私聊消息时触发
        public static void OnFriendMessageReceived(BotContext Bot, FriendMessageEvent @event)
        {

        }

        // 收到群聊消息时触发
        public static void OnGroupMessageReceived(BotContext Bot, GroupMessageEvent @event)
        {

        }

        // 收到群临时消息时触发
        public static void OnTempMessageReceived(BotContext Bot, TempMessageEvent @event)
        {

        }

        // 群管变更时触发
        public static void OnGroupAdminChangedEvent(BotContext Bot, GroupAdminChangedEvent @event)
        {

        }

        // 有人入群时触发
        public static void OnGroupMemberIncreaseEvent(BotContext Bot, GroupMemberIncreaseEvent @event)
        {

        }

        // 有人退群时触发
        public static void OnGroupMemberDecreaseEvent(BotContext Bot, GroupMemberDecreaseEvent @event)
        {

        }

        // 有好友申请时触发
        public static void OnFriendRequestEvent(BotContext Bot, FriendRequestEvent @event)
        {
            Console.WriteLine($"有新好友申请,账号:{@event.SourceUin.ToString()},昵称:{@event.Source},验证消息:{@event.Message}");
        }
    }
}
