IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[TataGreenNumberInsertUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[TataGreenNumberInsertUpdate]
GO

	

--Summary	: Insert Update TataGreen Numbers
--Author	: Dilip V. 13-Aug-2012

CREATE PROCEDURE [CRM].[TataGreenNumberInsertUpdate]
@CBDId			NUMERIC(18,0),
@GreenNumber	VARCHAR(200),
@UpdatedBy		NUMERIC(18,0),
@IsUpdated		BIT OUTPUT
AS 
BEGIN
	SET NOCOUNT ON
	
	IF NOT EXISTS (SELECT ID FROM CRM.TataGreenNumbers TGN WHERE Number = @GreenNumber)
	BEGIN
		IF NOT EXISTS (SELECT ID FROM CRM.TataGreenNumbers TGN WHERE CBDId = @CBDId)
		BEGIN
			INSERT INTO CRM.TataGreenNumbers 
					(CBDId,Number,UpdatedBy,UpdatedDate)
			VALUES (@CBDId,@GreenNumber,@UpdatedBy,GETDATE())
			SET @IsUpdated = 1;
		END
		ELSE
		BEGIN
			UPDATE CRM.TataGreenNumbers
			SET Number =  @GreenNumber, UpdatedBy = @UpdatedBy, UpdatedDate = GETDATE()
			WHERE CBDId = @CBDId
			SET @IsUpdated = 1;
		END
	END
	ELSE
		SET @IsUpdated = 0;
END

