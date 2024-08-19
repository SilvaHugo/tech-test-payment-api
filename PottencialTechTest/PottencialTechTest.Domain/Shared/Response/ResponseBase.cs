namespace PottencialTechTest.Domain.Shared.Response
{
    public class ResponseBase
    {
        public bool Sucesso { get; private set; } = false;
        public string? Mensagem { get; set; }
        public object? Body { get; private set; }

        public void SetSucesso()
        {
            Sucesso = true;
            Mensagem = "Operação realizada com sucesso.";
        }

        public void SetSucesso(object body)
        {
            Sucesso = true;
            Mensagem = "Operação realizada com sucesso.";
            Body = body;
        }

        public void SetBody(object body)
        {
            Body = body;
        }
    }
}
