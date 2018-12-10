IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ModifyRewardPointsRedemptionReqType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ModifyRewardPointsRedemptionReqType]
GO

	

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <4th Aug 2015>
-- Description:	<Update RewardPoints Redemption ReqType>
-- Updated By : vinay Kumar Prajapati (11th jan 2015 ) 
-- Purpose    : Added @TC_EWalletsId parameter  2- PayUmoney And 1- FlipKart Voucher
-- Updated By : Vinay Kumar Prajapati 18th Feb 2016 Get Actual redeemtion Amount.
-- =============================================
CREATE PROCEDURE [dbo].[TC_ModifyRewardPointsRedemptionReqType]
	@TC_RedeemedPointsId	INT,
	@RequestType		INT,
	@Comments			VARCHAR(300) = null,
	@ApprovalDate		DATETIME = null,
	@SentDate			DATETIME = null,
	@VoucherNo			VARCHAR(250) = null,
	@PIN				VARCHAR(100)= null,
	@ExpiryDate			DATETIME = null,
	@EmailId			VARCHAR(50),
	@UpdatedBy			INT,
	@TC_EWalletsId      smallInt = NULL -- 2 for PayUmoney And 1- FlipKart Voucher

AS
DECLARE @TransactionId Int
DECLARE @RedeemAmount Int
DECLARE @UserId Int 

DECLARE @UserEmailId  Varchar(100) -- For wallet 
DECLARE @UserMobileNo VARCHAR(15)      -- For wallet 

BEGIN
	UPDATE TC_RedeemedPoints 
	SET RequestType = @RequestType , ApprovalDate = CASE WHEN @SentDate IS NOT NULL AND @ApprovalDate IS NULL THEN ApprovalDate ELSE @ApprovalDate END,
	SentDate = @SentDate , PIN = @PIN , VoucherNo = @VoucherNo,ExpiryDate = @ExpiryDate,EmailSentOn = @EmailId
	WHERE Id = @TC_RedeemedPointsId

	INSERT INTO TC_RedeemedPointsLog (TC_RedeemedPointsId,Comment,RequestType,ActionTakenOn,ActionTakenBy)
	VALUES (@TC_RedeemedPointsId,@Comments,@RequestType,GETDATE(),@UpdatedBy)
    
	
	--- TC_EWalletTransactions insert Data 
	IF @TC_EWalletsId <> 1 -- For EWallet Transaction(PayUMoney) 
	BEGIN 
		INSERT INTO TC_EWalletTransactions (TC_RedeemedPointsId,ApprovedBy,ApprovedOn) VALUES (@TC_RedeemedPointsId,@UpdatedBy,GETDATE())
		SET @TransactionId =  SCOPE_IDENTITY()
        
		SELECT  @RedeemAmount = RP.RedeemedAmount,@UserEmailId = UAD.EmailId,@UserMobileNo=UAD.MobileNo
	    FROM   TC_RedeemedPoints AS RP WITH(NOLOCK) 
		INNER JOIN TC_UsersEWalletAccountDetails AS UAD WITH(NOLOCK) ON UAD.Tc_UsersId =RP.UserId AND UAD.TC_EWalletsId=@TC_EWalletsId
		WHERE   RP.Id=@TC_RedeemedPointsId

     END

	  SELECT  @TransactionId AS TransactionId ,@RedeemAmount AS RedeemAmount ,@UserEmailId AS UserEmail, @UserMobileNo AS UserMobile

END




