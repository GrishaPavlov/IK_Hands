using System.Collections;

namespace PopovRadio.Scripts.Common
{
    public interface IState
    {
        public IEnumerator Start();
    }
}