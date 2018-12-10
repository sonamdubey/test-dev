IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[UpdateTopLevelLeadScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[UpdateTopLevelLeadScore]
GO

	

 -- =============================================
-- Author:        Naresh Palaiya
-- Create date: 09/07/2014
-- Description:    Update the Top Level Lead Score for corresponding pqid in the pq_clientinfo table
-- =============================================

Create PROCEDURE [CRM].[UpdateTopLevelLeadScore]
    -- Add the parameters for the stored procedure here
    @pqID bigint,
    @TopLevelLeadScore float  
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;   
--Update the TopLevelLead score into the pq_clientinfo table
    UPDATE pqc
    SET TopLevelLeadScore=@TopLevelLeadScore
    FROM pq_clientinfo pqc where pqc.PQId = @pqID           
END
 

