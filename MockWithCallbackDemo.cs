using Moq;
using System;
using System.Collections.Generic;
using MockWithCallbackDemo.Strategies;

namespace MockWithCallbackDemo
{
    public class MockWithCallbackDemo
    {
        private IDictionary<long, IStrategy> testTargetCollection;

        public MockWithCallbackDemo()
        {
            testTargetCollection = MockAndSetupTrygetOnDictionary();
        }

        public void Test()
        {            
            IStrategy strategy;
            testTargetCollection.TryGetValue(0, out strategy);
            Console.WriteLine(strategy.GetType());

            testTargetCollection.TryGetValue(1, out strategy);
            Console.WriteLine(strategy.GetType());
        }

        private IDictionary<long, IStrategy> MockAndSetupTrygetOnDictionary()
        {
            var dictionaryMock = new Mock<IDictionary<long, IStrategy>>();
            IStrategy strategy = null;
            dictionaryMock.Setup(foo => foo.TryGetValue(It.IsAny<long>(), out strategy))
                .Returns(new SelectStrategy((long longValue, out IStrategy strat) =>
                {
                    if (longValue == 0)
                    {
                        strat = new OtherStrategy();
                        return true;
                    }

                    strat = new GeneralStategy();
                    return true;
                }));

            return dictionaryMock.Object;
        }

        private delegate bool SelectStrategy(long longValue, out IStrategy strat);
    }
}
