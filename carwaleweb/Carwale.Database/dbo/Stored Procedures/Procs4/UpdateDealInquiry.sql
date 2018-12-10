IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealInquiry]
GO

	

-- =============================================
-- Author:		Jitendra	
-- Create date: 1/7/2016	
-- Description:	UpdateDealInquiry(Update push status for records)
-- =============================================

CREATE PROCEDURE [dbo].[UpdateDealInquiry]
	 @Id			INT,
	 @PushStatus 	INT,
	 @IsPaid        BIT = 0	
 AS

BEGIN
		UPDATE DealInquiries 
		SET PushStatus = @PushStatus,
			isPaid = @IsPaid
		WHERE ID = @Id
	
	
END


/****** Object:  StoredProcedure [dbo].[SaveDealInquiry-v16.5.5]    Script Date: 6/24/2016 11:27:41 AM ******/
SET ANSI_NULLS ON
