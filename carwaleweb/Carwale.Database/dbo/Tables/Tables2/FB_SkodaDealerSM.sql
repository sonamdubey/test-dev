CREATE TABLE [dbo].[FB_SkodaDealerSM] (
    [Id]        NUMERIC (18) NOT NULL,
    [DealerId]  NUMERIC (18) NOT NULL,
    [ASMName]   VARCHAR (50) NULL,
    [ASMEMail]  VARCHAR (50) NULL,
    [ASMMobile] VARCHAR (50) NULL,
    [RSMName]   VARCHAR (50) NULL,
    [RSMEMail]  VARCHAR (50) NULL,
    [RSMMobile] VARCHAR (50) NULL,
    [ZSMName]   VARCHAR (50) NULL,
    [ZSMEMail]  VARCHAR (50) NULL,
    [ZSMMobile] VARCHAR (50) NULL,
    CONSTRAINT [PK_FB_SkodaDealerSM] PRIMARY KEY CLUSTERED ([Id] ASC)
);

