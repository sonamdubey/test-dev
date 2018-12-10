CREATE TABLE [dbo].[Ins_Requests] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InsPoolId]       NUMERIC (18) NOT NULL,
    [InsuredValue]    NUMERIC (18) NULL,
    [PremiumAmount]   NUMERIC (18) NULL,
    [PaymentModeId]   INT          NULL,
    [RequestDateTime] DATETIME     NOT NULL,
    [Status]          BIT          CONSTRAINT [DF_Ins_Requests_Status] DEFAULT ((0)) NOT NULL,
    [Paid]            BIT          CONSTRAINT [DF_Ins_Requests_Paid] DEFAULT ((0)) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Ins_Requests_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Ins_Requests] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

