namespace FrameWork
{
    public interface ISingleton
    {
        
    }
    
    public abstract class MySingleton<T> : ISingleton where T : MySingleton<T>, new()
    {
        protected static T mInstance;
        static object mLock = new object();

        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new T();
                        mInstance.Init();
                    }
                }

                return mInstance;
            }
        }

        public virtual void Init()
        {
            
        }
    }
}