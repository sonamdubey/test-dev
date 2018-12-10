IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_ADM_PERules]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_ADM_PERules]
GO

	

-- =============================================
-- Author:		Jayant Mhatre
-- Create date: 15th July 2011
-- Description:	This proc insert data in NCD_Inquiries table
-- =============================================
CREATE PROCEDURE [dbo].[CRM_ADM_PERules]
	
(
	@ConsultantId   INT,
	@Make			INT,
	@Model			VARCHAR(200),
	@State			INT,
	@City			VARCHAR(200),
	@UserName		VARCHAR(50)
	)
	
	AS
BEGIN
	IF(@Model= '')
		BEGIN
			SET @Model='-1'
		END
	CREATE TABLE temp1
		(
			ConsultantId INT,Make Int,Model Int
		)
		CREATE TABLE temp2
		(
			Make Int,State INT,City Int,CreatedOn DateTime,UpdatedOn DateTime,UpdatedBy NVARCHAR(50)
		)
		CREATE TABLE temp3
		(
		 ConsultantId INT,State INT,Make Int,Model Int,City Int,CreatedOn DateTime,UpdatedOn DateTime,UpdatedBy NVARCHAR(50)
		)
		
			DECLARE @ModelIndx SMALLINT,@strModel VARCHAR(50),@CityIndx SMALLINT,@strCity VARCHAR(50)
  
			WHILE @Model <> ''                 
				BEGIN                
						SET @ModelIndx = CHARINDEX(',', @Model) 
						--to check if list id has ended                
						IF @ModelIndx > 0                
							BEGIN  
            
								SET @strModel = LEFT(@Model, @ModelIndx-1)  
								SET @Model = RIGHT(@Model, LEN(@Model)-@ModelIndx) 
                
								INSERT  temp1 (ConsultantId,Make,Model)
										values(@ConsultantId,@Make,@strModel)
							END                
						ELSE                
							BEGIN                
                
								SET @strModel = @Model  
								SET @Model = ''   
				                INSERT  temp1 (ConsultantId,Make,Model)
										values(@ConsultantId,@Make,@strModel)
							END         
				END
			
			 WHILE @City <> ''                 
				BEGIN                
						SET @CityIndx = CHARINDEX(',', @City) 
						--to check if list id has ended                
						IF @CityIndx > 0                
							BEGIN  
            
								SET @strCity = LEFT(@City, @CityIndx-1)  
								SET @City = RIGHT(@City, LEN(@City)-@CityIndx) 
                
								INSERT  temp2 (Make,State,City,CreatedOn,UpdatedOn,UpdatedBy)
										values(@Make,@State,@strCity,GETDATE(),GETDATE(),@UserName)
							END                
						ELSE                
							BEGIN                
                
								SET @strCity = @City  
								SET @City = ''   
				                INSERT  temp2 (Make,State,City,CreatedOn,UpdatedOn,UpdatedBy)
										values(@Make,@State,@strCity,GETDATE(),GETDATE(),@UserName)
							END         
				END
			
			 INSERT INTO temp3(ConsultantId,Make,Model,State,City,CreatedOn,UpdatedOn,UpdatedBy)  SELECT temp1.ConsultantId,temp1.Make,temp1.Model,temp2.State, temp2.City,temp2.CreatedOn,temp2.UpdatedOn,temp2.UpdatedBy FROM temp1 join temp2 ON temp1.Make=temp2.Make
			 
			 
			 DROP TABLE temp1
			 DROP TABLE temp2
			 
			 
			INSERT INTO CRM_ADM_PEConsultantRules
			SELECT ConsultantId ,Make,Model,State,City,CreatedOn,UpdatedOn,UpdatedBy
			FROM temp3
			WHERE NOT EXISTS(SELECT * 
                 FROM CRM_ADM_PEConsultantRules 
                 WHERE (CRM_ADM_PEConsultantRules.ConsultantId=temp3.ConsultantId and CRM_ADM_PEConsultantRules.MakeId=temp3.Make and CRM_ADM_PEConsultantRules.ModelId = temp3.Model and CRM_ADM_PEConsultantRules.StateId = temp3.State and  CRM_ADM_PEConsultantRules.CityId=temp3.City)
                 )
                 
                 DROP TABLE temp3
                 
                 
			 
END
						
							
	
	
	

