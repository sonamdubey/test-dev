IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerBankSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerBankSave]
GO

	-- =============================================
-- Author:		SURENDRA CHOUKSEY
-- Create date: 5th October 2011
-- Description:	This procedure is used to add update dealer's Bank
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerBankSave]
(
@TC_DealerBank_Id INT =NULL,
@DealerId NUMERIC,
@BankName VARCHAR(50)
)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ReturnVal SMALLINT
	
	IF(@TC_DealerBank_Id IS NULL) --Insering Dealer's Bank
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_DealerBank WHERE DealerId=@DealerId AND BankName=@BankName AND IsActive=1)
		BEGIN
			INSERT TC_DealerBank(BankName,DealerId) VALUES(@BankName,@DealerId)			
			SET @ReturnVal= 0
		END
		ELSE
		BEGIN
			SET @ReturnVal=  -2 -- Means Duplicate record is already exists in DB
		END		
	END
	ELSE --  Updating Dealer's Bank
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_DealerBank WHERE DealerId=@DealerId AND TC_DealerBank_Id<>@TC_DealerBank_Id AND BankName=@BankName AND IsActive=1)
		BEGIN
			UPDATE TC_DealerBank SET BankName=@BankName WHERE TC_DealerBank_Id=@TC_DealerBank_Id AND DealerId=@DealerId
			RETURN 0
		END
		ELSE
		BEGIN
			SET @ReturnVal=  -2 -- Means Duplicate record is already exists in DB
		END		
	END
	SELECT TC_DealerBank_Id ,BankName FROM TC_DealerBank WHERE DealerId=@DealerId AND IsActive=1
	RETURN @ReturnVal
    
END

