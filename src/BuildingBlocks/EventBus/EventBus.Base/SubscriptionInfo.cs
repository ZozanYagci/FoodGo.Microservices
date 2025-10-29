using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base
{
    // her bir event için hangi handler'ın devreye girdiğini tutar.
    public sealed class SubscriptionInfo
    {
        public Type HandlerType { get; }

        private SubscriptionInfo(Type handlerType)
        {
            HandlerType = handlerType;
        }

        public static SubscriptionInfo Typed(Type handlerType)
        {
            if (handlerType == null)
                throw new ArgumentNullException(nameof(handlerType));

            return new SubscriptionInfo(handlerType);
        }

        public override string ToString() => HandlerType.Name;

    }
}
