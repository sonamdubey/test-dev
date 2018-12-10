IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_BlockDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_BlockDealer]
GO

	
-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 3rd May 2012
-- Description:	It will insert/update the CRM.FLCDealerPriority table but will not allow the duplicate entry
-- =============================================


      
CREATE PROCEDURE [dbo].[CRM_BlockDealer]      
@Dealer INT,      
@Model  INT,    
@Priorty INT,
@User INT,
@Status   NUMERIC OUTPUT       

      
AS      
       
BEGIN
			IF EXISTS (SELECT Id FROM CRM.FLCDealerPriority  WHERE ModelId = @Model AND DealerId = @Dealer AND Priority = @Priorty )
				BEGIN
					SET @Status = 0   -- for duplicate entry
				END	
			ELSE	      
				BEGIN 
					
					INSERT INTO CRM.FLCDealerPriority(ModelId,DealerId,Priority,UpdatedDate,UpdatedBy)
					VALUES(@Model,@Dealer,@Priorty,GETDATE(),@User)
		
					SET @Status = 1   -- for insert

			END    
END

