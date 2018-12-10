IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveClientTarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveClientTarget]
GO

	-- =============================================
-- Author:		Amit Yadav(7th Oct 2015)
-- Description:	To Save ESM_ClientTarget
-- =============================================
CREATE PROCEDURE [dbo].[ESM_SaveClientTarget]
	@ID INT,
	@Target NUMERIC(18,0),
	@Type SMALLINT,
	@OrgId INT,
	@FinancialYear VARCHAR(50),
	@UpdatedBy INT,
	@RetVal AS NUMERIC(18,0) OUT

AS

BEGIN

	 --Avoid Duplicate Entry
	 SET @RetVal= -1
	 SELECT * FROM ESM_ClientTargets AS ECT WITH(NOLOCK) WHERE ECT.Type=@Type AND ECT.OrgId=@OrgId
	 IF @@ROWCOUNT = 0 AND @ID=-1

		 BEGIN
				INSERT INTO ESM_ClientTargets(Type, OrgId, FinancialYear, Target, UpdatedBy)
				VALUES( @Type, @OrgId, @FinancialYear, @Target, @UpdatedBy)
			
				SET @RetVal = SCOPE_IDENTITY()
		 END
		 
	 ELSE

		BEGIN
		SELECT * FROM ESM_ClientTargets AS ECT WITH(NOLOCK) WHERE ECT.Type=@Type AND ECT.OrgId=@OrgId AND ECT.FinancialYear=@FinancialYear AND ECT.Target<>@Target
		IF @@ROWCOUNT = 0
		BEGIN
				UPDATE ESM_ClientTargets SET Target=@Target, UpdatedBy=@UpdatedBy 
				WHERE  Id=@ID
			
				SET @RetVal = @ID
		 END
	END
END
