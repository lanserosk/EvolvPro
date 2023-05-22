namespace EvolvPro.Models
{
    public class DataObserver : IObserver
    {
        private EvolvProContext context;

        public DataObserver(EvolvProContext context)
        {
            this.context = context;
        }

        public void Update()
        {
            // Realiza las acciones necesarias cuando ocurra un cambio relevante en los datos
            // Por ejemplo, puedes recargar los datos actualizados en tu aplicación
            // o realizar alguna otra lógica específica
        }

    }
}
