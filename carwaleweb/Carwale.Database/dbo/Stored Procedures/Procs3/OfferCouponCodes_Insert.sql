IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OfferCouponCodes_Insert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OfferCouponCodes_Insert]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 19/11/2014
-- Description: Generate Unique Coupen Code And Insert into table "OfferCouponCodes"
-- Execution: EXEC [dbo].[OfferCouponCodes_Insert] 5,7,2
-- =============================================
CREATE PROCEDURE [dbo].[OfferCouponCodes_Insert]
@OfferId NUMERIC(18,0),
@ReferenceId VARCHAR(10),
@OfferType INT,
@CouponId INT OUTPUT,
@CouponCode varchar(6) OUTPUT
AS
BEGIN
	DECLARE @UniqueCouponId varchar(6)
	SET     @UniqueCouponId = SUBSTRING(REPLACE(NEWID(), '-', ''), 1, 6) 
                                                    
    -------------While condition added by Manish on 02-09-2013 for implementing alphnumeric Unique customer id------------
           WHILE  EXISTS (SELECT CouponCode FROM OfferCouponCodes WITH (NOLOCK) WHERE CouponCode=@UniqueCouponId)
           BEGIN 
              
              INSERT INTO PQ_FailedCoupon(UniqueCouponIdFailed,CreatedOn) VALUES (@UniqueCouponId,GETDATE())
              SET @UniqueCouponId = SUBSTRING(REPLACE(NEWID(), '-', ''), 1, 6) 
              
           END

	INSERT INTO OfferCouponCodes (OfferId,ReferenceId,CouponCode,OfferType,GeneratedOn) Values(@OfferId,@ReferenceId,@UniqueCouponId,@OfferType,GETDATE())
	
	SET @CouponId = SCOPE_IDENTITY();
	SET @CouponCode =  @UniqueCouponId

END
