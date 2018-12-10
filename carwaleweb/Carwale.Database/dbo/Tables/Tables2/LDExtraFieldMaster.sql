CREATE TABLE [dbo].[LDExtraFieldMaster] (
    [ID]        NUMERIC (18)  NOT NULL,
    [LDTakerId] NUMERIC (18)  NOT NULL,
    [LDType]    SMALLINT      NOT NULL,
    [FieldName] VARCHAR (100) NULL,
    [IsActive]  BIT           CONSTRAINT [DF_LDExtraFieldMaster_IsActive] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK_LDExtraFieldMaster] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

