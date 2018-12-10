CREATE TABLE [dbo].[TC_DealerAdmin] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Organization]  VARCHAR (60)  NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_TC_DealerAdmin_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]     DATETIME      NULL,
    [EmailId]       VARCHAR (250) NULL,
    [IsEnquiryMail] BIT           NULL,
    [DealerId]      INT           NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_TC_DealerAdmin_CreatedOn] DEFAULT (getdate()) NULL,
    [IsGroup]       BIT           NULL,
    CONSTRAINT [PK_TC_DealerAdmin] PRIMARY KEY CLUSTERED ([Id] ASC)
);

