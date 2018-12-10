CREATE TABLE [dbo].[Con_NewEditCms_OtherInfo] (
    [ID]              NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [BasicId]         NUMERIC (18)    NULL,
    [CategoryFieldId] NUMERIC (18)    NULL,
    [BooleanValue]    BIT             NULL,
    [NumericValue]    NUMERIC (18)    NULL,
    [DecimalValue]    DECIMAL (18, 2) NULL,
    [TextValue]       VARCHAR (2500)  NULL,
    [DateTimeValue]   DATETIME        NULL,
    [ValueType]       NUMERIC (18)    NULL,
    [LastUpdatedTime] DATETIME        NULL,
    [LastUpdatedBy]   NUMERIC (18)    NULL,
    CONSTRAINT [PK_Con_NewEditCms_OtherInfo] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

