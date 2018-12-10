IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_UpdateReplacementLeadsDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_UpdateReplacementLeadsDetails]
GO

	
-- =============================================
-- Author:		Sunil Yadav 
-- Create date: 23 Dec 2015
-- Description:	Insert Or Update rejected/accepted lead in TC_ReplacementLeadDetails Table.
-- EXEC DCRM_UpdateReplacementLeadsDetails NULL,2505,1,'accepted'
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_UpdateReplacementLeadsDetails] 
	@ContractId INT = NULL,
	@InquiryLeadId INT ,
	@Status INT = NULL,
	@Comment VARCHAR(MAX) = NULL,
	@TaggedBy INT = NULL
AS
BEGIN
	
	IF (@ContractId IS NULL) --to show cities on popup 
		BEGIN
			SELECT C.Name AS CustomerCity , CT.Name AS ActualCity   FROM TC_NewCarInquiries TNI WITH(NOLOCK)
			JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = TNI.TC_InquiriesLeadId AND TNI.TC_InquiriesLeadId = @InquiryLeadId
			JOIN TC_CustomerDetails TCD WITH(NOLOCK) ON TCD.Id = TIL.TC_CustomerId
			LEFT JOIN Cities C WITH(NOLOCK) ON C.ID = TCD.City 
			LEFT JOIN Cities CT WITH(NOLOCK) ON CT.ID = TNI.CityId
		END 

	ELSE
		BEGIN
			IF EXISTS(SELECT TOP 1 InquiryLeadId FROM TC_ReplacementLeadDetails WITH(NOLOCK) WHERE InquiryLeadId = @InquiryLeadId)
				BEGIN
					UPDATE TC_ReplacementLeadDetails
					SET Status = @Status, Comment=@Comment ,TagDate = GETDATE(),TaggedBy = @TaggedBy
					WHERE ContractId = @ContractId AND InquiryLeadId = @InquiryLeadId
				END
			ELSE 
				BEGIN
					INSERT INTO TC_ReplacementLeadDetails(ContractId,InquiryLeadId,Status,Comment,TagDate,TaggedBy) VALUES (@ContractId,@InquiryLeadId,@Status,@Comment,GETDATE(),@TaggedBy)
				END
		END
		

END


----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

SET ANSI_NULLS ON
