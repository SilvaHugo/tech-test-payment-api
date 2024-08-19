USE [PottencialSeguradora]
GO

/****** Object:  Table [dbo].[Produto]    Script Date: 8/19/2024 3:12:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Produto](
	[Id] [uniqueidentifier] NOT NULL,
	[CriadoEm] [datetime] NULL,
	[AlteradoEm] [datetime] NULL,
	[NomeProduto] [varchar](255) NOT NULL,
	[ValorProduto] [decimal](9, 2) NOT NULL,
	[VendaId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

USE [PottencialSeguradora]
GO

/****** Object:  Table [dbo].[Venda]    Script Date: 8/19/2024 3:12:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Venda](
	[Id] [uniqueidentifier] NOT NULL,
	[CriadoEm] [datetime] NULL,
	[AlteradoEm] [datetime] NULL,
	[Identificador] [varchar](255) NOT NULL,
	[DataVenda] [datetime] NOT NULL,
	[VendedorId] [uniqueidentifier] NOT NULL,
	[StatusVenda] [int] NOT NULL
) ON [PRIMARY]
GO

USE [PottencialSeguradora]
GO

/****** Object:  Table [dbo].[Vendedor]    Script Date: 8/19/2024 3:12:42 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Vendedor](
	[Id] [uniqueidentifier] NOT NULL,
	[CriadoEm] [datetime] NULL,
	[AlteradoEm] [datetime] NULL,
	[Nome] [varchar](255) NOT NULL,
	[Telefone] [bigint] NOT NULL,
	[Cpf] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Vendedor]
           ([Id]
           ,[CriadoEm]
           ,[AlteradoEm]
           ,[Nome]
           ,[Telefone]
           ,[Cpf]
           ,[Email])
     VALUES
           ('D851C0BF-1D5D-4E25-9AC4-52495B6DB65C'
           ,GETDATE()
           ,NULL
           ,'Vendedor Teste'
           ,3124496858
           ,'19179297064'
           ,'teste@email.com')