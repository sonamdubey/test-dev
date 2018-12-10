IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BWLeadInqStatusChangeLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BWLeadInqStatusChangeLog]
GO

	-- ============================================================
--  Author		: Suresh Prajapati
--  Created on	: 27th Jan, 2016
--  Description : To Log BikeWale Lead Inquiry Status change
-- ============================================================
CREATE PROCEDURE [dbo].[TC_BWLeadInqStatusChangeLog] @TC_InqLeadId INT,@BWLeadInqStatus TINYINT
	,@CreatedBy INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TC_BWLeadStatusLog (
		TC_InquiryLeadId
		,TC_BWLeadStatusId
		,CreatedBy
		,CreatedOn
		)
	VALUES (
		@TC_InqLeadId
		,@BWLeadInqStatus
		,@CreatedBy
		,GETDATE()
		)
END

