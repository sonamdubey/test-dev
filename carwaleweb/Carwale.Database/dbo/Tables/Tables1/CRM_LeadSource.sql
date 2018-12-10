CREATE TABLE [dbo].[CRM_LeadSource] (
    [LeadId]     NUMERIC (18)  NOT NULL,
    [CategoryId] SMALLINT      NOT NULL,
    [SourceId]   NUMERIC (18)  NOT NULL,
    [SourceName] VARCHAR (150) NOT NULL,
    [EntryDate]  DATETIME      NOT NULL,
    CONSTRAINT [PK_CRM_LeadSource] PRIMARY KEY CLUSTERED ([LeadId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_CRM_LeadSource_CategoryId]
    ON [dbo].[CRM_LeadSource]([CategoryId] ASC)
    INCLUDE([SourceId], [SourceName], [EntryDate]);


GO

CREATE TRIGGER [dbo].[TRG_CRM_LeadSource]
ON [dbo].[CRM_LeadSource]
AFTER INSERT 
AS
IF update(SourceId)
BEGIN
declare @LeadId bigint
select @LeadId=LeadId
from inserted 
EXEC CRM.LSUpdateLeadScore 3, @LeadId, -1
END;

GO
DISABLE TRIGGER [dbo].[TRG_CRM_LeadSource]
    ON [dbo].[CRM_LeadSource];

