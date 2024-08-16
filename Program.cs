/*
 * 让QQ机器人更简单
 * 采用lagrange NTQQ框架
 */

namespace Program
{
    class Application
    {
        // 主程序入口
        public static async Task Main(string[] args)
        {
            var app = await EasyBot.Login.Begin();
        }
    }
}