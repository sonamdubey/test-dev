IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_ManageNewCarLTV]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_ManageNewCarLTV]
GO

	CREATE PROCEDURE [dbo].[CW_ManageNewCarLTV]
@CarModelId Numeric(18,0),
@IsUpdate Bit=0,
@LtvUpTo36 int,
@LtvUpTo60 int=NULL,
@LtvGT60 int=NULL,
@RecordExists bit OUTPUT,
@UpdatedBy INT = NULL,
@UpdatedOn DATETIME = NULL

AS 
--Author:Rakesh Yadav on 1 Aug 2015
--desc: insert and update new car ltv for hdfc
BEGIN
	SET @RecordExists=1
	IF NOT EXISTS(Select Id FROM CW_NewCarLTV WHERE CarModelId=@CarModelId) AND @IsUpdate=0
	BEGIN
		DECLARE @Tenor int
		SET @Tenor =1
		WHILE @Tenor<=3
		BEGIN
			INSERT INTO CW_NewCarLTV(CarModelId,Tenor,LTV,IsActive, UpdatedBy)
			VALUES(@CarModelId,@Tenor,@LtvUpTo36,1, @UpdatedBy)

			SET @Tenor=@Tenor+1
		END

		 INSERT INTO CW_NewCarLTV(CarModelId,Tenor,LTV,IsActive, UpdatedBy)
		 VALUES(@CarModelId,4,@LtvUpTo60,1, @UpdatedBy),(@CarModelId,5,@LtvUpTo60,1, @UpdatedBy),(@CarModelId,6,@LtvGT60,1, @UpdatedBy),(@CarModelId,7,@LtvGT60,1, @UpdatedBy)
		
		 SET @RecordExists=0
		
	END
	ELSE
	BEGIN
		IF @IsUpdate=1 
			BEGIN
			UPDATE CW_NewCarLTV
			SET LTV=@LtvUpTo36, IsActive=1,
				UpdatedBy = @UpdatedBy,
				UpdatedOn = @UpdatedOn
			WHERE CarModelId=@CarModelId AND Tenor IN(1,2,3)

			UPDATE CW_NewCarLTV
			SET LTV=@LtvUpTo60, IsActive=1,
				UpdatedBy = @UpdatedBy,
				UpdatedOn = @UpdatedOn
			WHERE CarModelId=@CarModelId AND Tenor IN(4,5)

			UPDATE CW_NewCarLTV
			SET LTV=@LtvGT60, IsActive=1,
				UpdatedBy = @UpdatedBy,
				UpdatedOn = @UpdatedOn
			WHERE CarModelId=@CarModelId AND Tenor IN(6,7)
			SET @RecordExists=1
		END
	END
END