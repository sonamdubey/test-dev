IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InsertLeadSource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InsertLeadSource]
GO

	-- =============================================
-- Author:		Rajeev
-- Create date: 17th Aug 2010
-- Description:	This proc saves the lead source data to the database
-- =============================================
CREATE PROCEDURE [dbo].[CRM_InsertLeadSource]
	@LeadId			INTEGER,
	@CategoryId		SMALLINT,
	@SourceId		NUMERIC,
	@SourceName		VARCHAR(150),
	@EntryDate		DATETIME
AS
BEGIN
	
	Insert Into CRM_LeadSource
			(LeadId, CategoryId, SourceId, SourceName, EntryDate)
	Values
			(@LeadId, @CategoryId, @SourceId, @SourceName, @EntryDate)
	
END
