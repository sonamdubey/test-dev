CREATE TABLE [dbo].[TC_DealerType] (
    [TC_DealerTypeId] TINYINT      NULL,
    [DealerType]      VARCHAR (50) NULL,
    [IsActive]        BIT          CONSTRAINT [DF_TC_DealerType_IsActive] DEFAULT ((1)) NULL
);

