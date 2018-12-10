IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_UpdateShowroomServiceRecord]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_UpdateShowroomServiceRecord]
GO

	
CREATE PROCEDURE [dbo].[OLM_UpdateShowroomServiceRecord]
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
		         UPDATE  OLM_ShowroomDetails
                 SET 
					 CityId=@CityId,
					 SOutlet =@SOutlet,
					 OutletCode = @OutletCode,
					 SAddress=@SAddress,
					 SContactNo= @SContactNo,
					 SEmail=@SEmail,
					 SFax=@SFax,
					 IsRapidOutlet =@IsRapidOutlet,
					 Lattitude= @Lattitude ,
					 Longitude=@Longitude ,
					 UpdatedBy=@UpdatedBy ,
					 UpdatedOn=@UpdatedOn
                 WHERE Id = @Id
		    
			END
	    ELSE IF @Type=2
	        BEGIN
	              UPDATE  OLM_ServiceCenterDetails
                 SET 
					 CityId=@CityId,
					 SOutlet =@SOutlet,
					 OutletCode = @OutletCode,
					 SAddress=@SAddress,
					 SContactNo= @SContactNo,
					 SEmail=@SEmail,
					 SFax=@SFax ,
					 IsRapidOutlet =@IsRapidOutlet,
					 Lattitude= @Lattitude ,
					 Longitude=@Longitude ,
					 UpdatedBy=@UpdatedBy ,
					 UpdatedOn=@UpdatedOn
                 WHERE Id = @Id
	        
	        END
	        
	         ELSE IF @Type=3
	        BEGIN
	              UPDATE  OLM_DealerDetails
                 SET 
					 CityId=@CityId,
				     DealerPrinciple=  @DealerPrinciple,
				     DMobileNoAndEmail=@DpMoblieNumber,
				     ContactPerson=@ContactPerson,
				     CMobileNoAndEmail=@CpMobileNumber,
					 UpdatedBy=@UpdatedBy ,
					 UpdatedOn=@UpdatedOn
                 WHERE Id = @Id
	        
	        END
	    
	END