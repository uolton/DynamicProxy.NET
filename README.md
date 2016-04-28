# DynamicProxy.NET
.NET动态代理

Demo:


        static void Main(string[] args)
        {

            var proxyMotheds = new Dictionary<string, DynamicAction>();

            // key is  Proxy's methodName, value is Actions
            proxyMotheds.Add("Add", new DynamicAction()
            {
                BeforeAction = new Action(() => Console.WriteLine("Before Doing....")),
                AfterAction = new Action(() => Console.WriteLine("After Doing...."))
            });

            var user = new User();
            //proxy for User
            var t = ProxyFactory<User>.Create(user, proxyMotheds);

            int count = 0;

            t.Add("Tom", 28, out count);

            t.SayName();

            Console.WriteLine(count);
            Console.Read();


        }
        
        
        
