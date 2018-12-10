IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[AddUpdateCarUSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[AddUpdateCarUSP]
GO

	




--Summary	: THIS PROCEDURE IS FOR INSERTING AND UPDATING RECORDS FOR CarUSP
--Author	: Dilip V. 31-Jul-2012

CREATE PROCEDURE [CRM].[AddUpdateCarUSP]
	@ID					NUMERIC,			--ID. IF IT IS -1 THEN IT IS FOR INSERT, ELSE IT IS FOR UPDATE FOR THE ID 		        	
	@MakeId				NUMERIC = NULL,				
	@ModelId			NUMERIC = NULL,			
	@IsActive			BIT,
	@Title				VARCHAR(200),		
	@Desciption 		VARCHAR(MAX),			
	@UpdatedBy			NUMERIC
 AS
	
BEGIN
	SET NOCOUNT ON
	IF @ID = -1
	BEGIN
		--IT IS FOR THE INSERT
		IF NOT EXISTS(SELECT Id FROM CRM.CarUSP WHERE ModelId = @ModelId)
		BEGIN
		INSERT INTO CRM.CarUSP(MakeId,  ModelId, 	IsActive, 	Title, 	USPDescription,	 UpdatedBy,  UpdatedOn) 
						VALUES(@MakeId, @ModelId, @IsActive,@Title, @Desciption, @UpdatedBy, GETDATE())
		END
	END
	ELSE
	BEGIN
		--IT IS FOR THE UPDATE
		UPDATE CRM.CarUSP 
		SET 
			IsActive		= @IsActive,
			Title			= @Title,
			USPDescription	= @Desciption,
			UpdatedBy		= @UpdatedBy,	
			UpdatedOn		= GETDATE()
		 WHERE 
			Id = @ID

	END
	
		
END





