IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[UpdateDeleteCarUSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[UpdateDeleteCarUSP]
GO

	


--Summary	: THIS PROCEDURE IS FOR UPDATING AND DELETE RECORDS FOR CarUSP
--Author	: Dilip V. 31-Jul-2012

CREATE PROCEDURE [CRM].[UpdateDeleteCarUSP]		
	@IsActive			BIT = NULL,	
	@Id			 		VARCHAR(MAX),			
	@UpdatedBy			NUMERIC
 AS
	
BEGIN
	SET NOCOUNT ON
		
	IF(@IsActive IS NOT NULL)
		BEGIN		
			UPDATE CRM.CarUSP 
			SET 
				IsActive		= @IsActive,
				UpdatedBy		= @UpdatedBy,	
				UpdatedOn		= GETDATE()
			 WHERE 
				Id IN (SELECT ListMember FROM fnSplitCSV(@ID))
		END
	ELSE
		BEGIN
			DELETE CRM.CarUSP WHERE Id IN (SELECT ListMember FROM fnSplitCSV(@ID))
		END
END

