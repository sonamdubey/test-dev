IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckBlankUseridInTC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckBlankUseridInTC]
GO

	-- =============================================
-- Author:      Vinayak
-- Create date: 29-10-2013
-- Description: Returns the no of leads in Autobiz with null UserID and stage is verification and consultation.
-- =============================================
CREATE PROCEDURE [dbo].[CheckBlankUseridInTC]        
AS
	BEGIN
	    SELECT CONVERT (DATE, CreatedDate) as CreatedDate,
		       COUNT(TC_InquiriesLeadId) as Count
		FROM TC_InquiriesLead WITH (NOLOCK)
		WHERE TC_LeadStageId IN (1,2) 
		AND TC_Userid IS NULL
		AND CreatedDate>'2013-10-25'
		GROUP BY  CONVERT (DATE, CreatedDate)
      
	END