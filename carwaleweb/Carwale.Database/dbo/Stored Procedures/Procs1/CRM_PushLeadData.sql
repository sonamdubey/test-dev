IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_PushLeadData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_PushLeadData]
GO

	

CREATE PROCEDURE [dbo].[CRM_PushLeadData]( @CRM_PushLeads CRM_PushLeads READONLY)
AS
BEGIN
    INSERT INTO CRM_PushedLeadLog
                (CustId,
                 PushedDate,                        
                 UpdatedBy,
                 LeadId,
                 InQuiryDate)
    SELECT CP.CustId,
           CP.PushedDate,
           CP.UpdatedBy,
           CP.LeadId,
           CP.InQuiryDate           
   FROM @CRM_PushLeads CP
 END
