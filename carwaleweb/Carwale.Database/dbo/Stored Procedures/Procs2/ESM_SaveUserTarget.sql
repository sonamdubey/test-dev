IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveUserTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveUserTarget]
GO

	
--Author : Vinay Kumar prajapati 12 desc 2014
-- Purpose : To Save ESM User  Target

CREATE PROCEDURE [dbo].[ESM_SaveUserTarget]
(
@UserId  INT,
@Target NUMERIC(18,0),
@FinancialYear Varchar(50),
@Quarter TINYINT,
@UpdatedBy INT,
@Status  TinyInt OUTPUT

)
AS
BEGIN
 
 SELECT EUT.Id FROM ESM_UserTargets AS EUT WITH(NOLOCK) WHERE EUT.UserId=@UserId AND EUT.FinancialYear=@FinancialYear AND EUT.Quarter=@Quarter AND EUT.IsActive=1
 IF @@ROWCOUNT <> 0

	 BEGIN
			UPDATE ESM_UserTargets SET Target=@Target , @UpdatedBy=@UpdatedBy ,UpdatedOn=GETDATE() 
			WHERE UserId=@UserId AND FinancialYear=@FinancialYear AND Quarter=@Quarter

			SET @Status= 2
	 END
 ELSE
	 BEGIN
			INSERT INTO ESM_UserTargets(UserId, FinancialYear,Quarter,Target,UpdatedBy, UpdatedOn, IsActive)
			VALUES( @UserId,@FinancialYear,@Quarter,@Target,@UpdatedBy,GetDate(), 1)
			
			SET @Status=1
	 END
	
END

