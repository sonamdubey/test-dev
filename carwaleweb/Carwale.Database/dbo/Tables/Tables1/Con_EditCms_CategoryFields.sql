CREATE TABLE [dbo].[Con_EditCms_CategoryFields] (
    [ID]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CategoryId]      NUMERIC (18)  NULL,
    [FieldName]       VARCHAR (100) NULL,
    [Priority]        INT           NULL,
    [ValueType]       INT           NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Con_EditCms_CategoryFields_IsActive] DEFAULT ((1)) NULL,
    [LastUpdatedTime] DATETIME      NULL,
    [LastUpdatedBy]   NUMERIC (18)  NULL,
    CONSTRAINT [PK_Con_EditCms_CategoryFields] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

