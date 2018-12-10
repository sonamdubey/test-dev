IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertTCDealerBranch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertTCDealerBranch]
GO

	CREATE Procedure [dbo].[InsertTCDealerBranch]
@DealerId Numeric,
@BranchName VarChar(100)
AS 
Begin
	Declare @branchID Numeric
	Set @branchID = ( Select TOP 1 ID From TC_Dealer_Branch Where DealerID = @DealerId )
	IF ( @branchID != 0 )
		Begin 
			Update TC_Dealer_Branch Set DealerId = @DealerId, BranchName = @BranchName Where Id = @branchId
		End
	Else
		Begin
			Insert Into TC_Dealer_Branch ( DealerID, BranchName ) Values ( @DealerId, @BranchName )
		End
End