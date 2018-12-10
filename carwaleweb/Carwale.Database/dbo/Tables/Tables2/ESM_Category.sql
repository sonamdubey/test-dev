CREATE TABLE [dbo].[ESM_Category] (
    [id]        NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Category]  VARCHAR (50) NOT NULL,
    [type]      SMALLINT     NOT NULL,
    [IsActive]  BIT          CONSTRAINT [DF_ESM_Category_IsActive] DEFAULT ((1)) NOT NULL,
    [UpdatedOn] DATETIME     NOT NULL,
    [UpdatedBy] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_ESM_Category] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

