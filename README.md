## O TESTE

Necessita criar uma API para manter vendas, com as seguintes regras:

1) Deve ser possível o registro de uma venda, que consiste nos dados do vendedor + itens vendidos; 
2) Uma venda contém informação sobre o vendedor que a efetivou, data, identificador do pedido e os itens que foram vendidos;
3) O vendedor deve possuir id, cpf, nome, e-mail e telefone;
4) A inclusão de uma venda deve possuir pelo menos 1 item;
5) Após o registro da venda ela deverá ficar com status "Aguardando Pagamento";
6) Deve ser possível obter uma venda através do seu ID;
7) Deve ser possível incluir novos itens ou remover itens, enquanto a venda ainda estiver com status "Aguardando Pagamento”, observando o item 4;
8) Deve ser possível atualizar o status de uma venda informando seu ID e algum dos status: 
`Pagamento aprovado` | `Enviado para transportadora` | `Entregue` | `Cancelada`;
 
9) Deve ser respeitada a seguinte regra de atualização de status:
 
De: `Aguardando pagamento` Para: `Pagamento Aprovado`
De: `Aguardando pagamento` Para: `Cancelada`
De: `Pagamento Aprovado` Para: `Enviado para Transportadora`
De: `Pagamento Aprovado` Para: `Cancelada`
De: `Enviado para Transportador` Para: `Entregue`
