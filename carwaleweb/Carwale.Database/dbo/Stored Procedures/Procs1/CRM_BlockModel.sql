IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_BlockModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_BlockModel]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 3rd May 2012
-- Description:	It will insert/update the CRM.FLCBlockType table but will not allow the duplicate entry
-- =============================================


      
CREATE PROCEDURE [dbo].[CRM_BlockModel]      
@City INT,      
@Model  INT,    
@Type INT,
@User INT,
@Status   NUMERIC OUTPUT       

      
AS      
       
BEGIN
			IF EXISTS (SELECT Id FROM CRM.FLCBlockType WHERE ModelId = @Model AND CityId = @City AND Type = @Type )
				BEGIN
					SET @Status = 0   -- for duplicate entry
				END	
			ELSE	      
				BEGIN 
					
					INSERT INTO CRM.FLCBlockType(ModelId,CityId,Type,UpdatedDate,UpdatedBy)
					VALUES(@Model,@City,@Type,GETDATE(),@User)
		
					SET @Status = 2   -- for insert

			END    
END

