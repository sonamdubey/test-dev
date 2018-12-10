IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_NCS_AddSkodaDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_NCS_AddSkodaDealer]
GO

	
-- =============================================
-- Author:		Jayant Mhatre 
-- Create date: 25th April 2012.
-- Description:	This proc Insert Or Update  data in CRM_SkodaDealers table
-- =============================================
CREATE PROCEDURE [dbo].[CRM_NCS_AddSkodaDealer]
	
	(
		@Id INT,
		@CityId INT,
		@DealerCode NVARCHAR(200),
		@KvpsCode NVARCHAR(200),
		@IsUpdate INT
	)
	AS
	BEGIN
		If @IsUpdate = 1
			BEGIN 
				UPDATE CRM_SkodaDealers 
				SET DealerCode=@DealerCode , DealerKVPSCode =@KvpsCode, CityId = @CityId
				WHERE DealerId =@Id
				
				UPDATE NCS_Dealers SET CityId = @CityId WHERE ID = @Id
				
				UPDATE CRM_SkodaDMSCities SET MappedCityId = @CityId WHERE CityId = @CityId
				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO CRM_SkodaDMSCities (CityId, MappedCityId) VALUES(@CityId, @CityId)
					END
				
			END
		ELSE
			BEGIN
				 INSERT INTO CRM_SkodaDealers(DealerId,CityId,DealerCode,DealerKVPSCode)
				 VALUES (@Id,@CityId,@DealerCode,@KvpsCode)
				 
				UPDATE CRM_SkodaDMSCities SET MappedCityId = @CityId WHERE CityId = @CityId
				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO CRM_SkodaDMSCities (CityId, MappedCityId) VALUES(@CityId, @CityId)
					END
			END
			
		
	END		
							
	
	
	
