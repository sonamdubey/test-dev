CREATE TABLE [dbo].[SMSSent_arch_0616] (
    [ID]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [Number]          VARCHAR (50)  NOT NULL,
    [Message]         VARCHAR (500) NOT NULL,
    [ServiceType]     INT           NOT NULL,
    [SMSSentDateTime] DATETIME      NOT NULL,
    [Successfull]     BIT           NOT NULL,
    [ReturnedMsg]     VARCHAR (500) NULL,
    [SMSPageUrl]      VARCHAR (500) NULL,
    [ServiceProvider] VARCHAR (250) NULL,
    [IsSMSSent]       BIT           NULL,
    CONSTRAINT [PK_SMSSent12] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_SMSSent12_Number]
    ON [dbo].[SMSSent_arch_0616]([Number] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SMS12_SMSSentDateTime]
    ON [dbo].[SMSSent_arch_0616]([SMSSentDateTime] ASC);

