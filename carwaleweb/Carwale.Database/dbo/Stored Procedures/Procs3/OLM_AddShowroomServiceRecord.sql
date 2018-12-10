IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_AddShowroomServiceRecord]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_AddShowroomServiceRecord]
GO

	-- =============================================
-- Author:		<RAHUL KUMAR>
-- Create date: <14-OCT-2013>
-- Description:	<ADD ShowroomService Recrod >
-- =============================================
CREATE PROCEDURE [dbo].[OLM_AddShowroomServiceRecord]
    (
    @Id NUMERIC(18,0),
    @Type NUMERIC(18,0),
    @CityId  Int,
    @UpdatedBy BigInt,
    @UpdatedOn DateTime,

   --these variable are used for OLM_ShowroomDetails OLM_ServiceCenterDetails  table
    @SOutlet varchar(50) = NUll,
    @OutletCode VarChar(50)=  NUll,
    @SAddress VarChar(500) = NUll,
    @SContactNo VarChar(500) = NUll,
    @SEmail VarChar(500) = NUll,
    @SFax VarChar(500) = NUll,
    @Lattitude VarChar(50) = NUll,
    @IsRapidOutlet Bit = NUll,
    @Longitude VarChar(50) = NUll,
    
  --this variable use for update DealerDetails 
    @DealerPrinciple VarChar(500) = NUll,
    @DpMoblieNumber VarChar(500) = NUll,
    @ContactPerson VarChar(500) = NUll,
    @CpMobileNumber VarChar(500) = NUll
   
	)
	
	AS
	
	BEGIN
	    IF @Type = 1
			BEGIN
		         INSERT INTO OLM_ShowroomDetails
                  (  SkodaDealerId,
				     CityId,
					 SOutlet,
					 OutletCode,
					 SAddress,
					 SContactNo, 
					 SEmail,
					 SFax,
					 IsRapidOutlet,
					 Lattitude,
					 Longitude,
					 UpdatedBy,
					 UpdatedOn)
					 VALUES
					 (@Id,
					  @CityId,
					  @SOutlet,
					  @OutletCode,
					  @SAddress,
					  @SContactNo,
					  @SEmail,
					  @SFax,
					  @IsRapidOutlet,
					  @Lattitude,
					  @Longitude,
					  @UpdatedBy,
					  @UpdatedOn
					  )
                
		    
			END
	    ELSE IF @Type=2
	        BEGIN
	              INSERT INTO OLM_ServiceCenterDetails
                    (
					 SkodaDealerId,
					 CityId,
					 SOutlet,
					 OutletCode,
					 SAddress,
					 SContactNo, 
					 SEmail,
					 SFax,
					 IsRapidOutlet, 
					 Lattitude,
					 Longitude,
					 UpdatedBy,
					 UpdatedOn
					 )
					 VALUES
					 (
					  @id,
					  @CityId,
					  @SOutlet,
					  @OutletCode,
					  @SAddress,
					  @SContactNo,
					  @SEmail,
					  @SFax ,
					  @IsRapidOutlet,
					  @Lattitude, 
					  @Longitude,
					  @UpdatedBy,
					  @UpdatedOn
					 )
             
	        
	        END
	        
	         ELSE IF @Type=3
	        BEGIN
	              INSERT INTO OLM_DealerDetails
                     (
					 SkodaDealerId,
					 CityId,
				     DealerPrinciple, 
				     DMobileNoAndEmail,
				     ContactPerson,
				     CMobileNoAndEmail,
					 UpdatedBy,
					 UpdatedOn
					 )
					 VALUES
                     (
					  @Id,
					  @CityId,
					  @DealerPrinciple,
					  @DpMoblieNumber,
					  @ContactPerson,
					  @CpMobileNumber,
					  @UpdatedBy ,
					  @UpdatedOn
					 )
	        
	        END
	     
	END
  
