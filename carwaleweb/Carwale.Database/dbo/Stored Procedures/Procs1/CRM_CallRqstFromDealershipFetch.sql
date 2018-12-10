IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CallRqstFromDealershipFetch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CallRqstFromDealershipFetch]
GO

	-- =============================================
-- Author:		AMIT KUMAR
-- Create date: 26th sept 2013
-- Description:	To fetch the data of call requested from dealer.
-- =============================================
CREATE PROCEDURE [dbo].[CRM_CallRqstFromDealershipFetch] 
@CbdId			NUMERIC(18,0),
@Dealerid		NUMERIC(18,0)
AS
BEGIN
	SELECT id,CallRequestDate, EventCompletedOn,isApproved FROM CRM_CustomerCallRqstLog WHERE CBDId = @CbdId AND DealerId = @Dealerid
END

