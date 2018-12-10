CREATE TABLE [dbo].[TC_DealerMapping] (
    [TC_DealerMappingId] INT      IDENTITY (1, 1) NOT NULL,
    [IsActive]           BIT      CONSTRAINT [DF_TC_DealerMapping_IsActive] DEFAULT ((1)) NULL,
    [EntryDate]          DATETIME CONSTRAINT [DF_TC_DealerMapping_EntryDate] DEFAULT (getdate()) NULL,
    [DealerAdminId]      INT      NOT NULL,
    [CreatedBy]          INT      NOT NULL,
    [ModifiedDate]       DATETIME NULL,
    [ModifiedBy]         INT      NULL,
    CONSTRAINT [PK_TC_DealerMapping] PRIMARY KEY CLUSTERED ([TC_DealerMappingId] ASC)
);

