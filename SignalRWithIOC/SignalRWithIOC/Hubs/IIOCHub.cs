using System.Threading.Tasks;

namespace SignalRWithIOC.Hubs
{
    public interface IIOCHub
    {
        void Pulse();
        void ServicePulse(string message);
    }
}