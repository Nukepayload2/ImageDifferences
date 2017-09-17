__kernel void ImageDiffBlend(read_only image2d_t imageA, read_only image2d_t imageB, write_only image2d_t imageC)
{
    const sampler_t sampler = CLK_NORMALIZED_COORDS_FALSE | CLK_ADDRESS_CLAMP | CLK_FILTER_NEAREST;

    int x = get_global_id(0);
    int y = get_global_id(1);

	int2 coord = (int2)(x, y); 

    uint4 A = read_imageui(imageA, sampler, coord);
    uint4 B = read_imageui(imageB, sampler, coord);
    uint4 C = 0;

    if(A.x != B.x || A.y != B.y || A.z != B.z)
	{
	    C=255-A;
		C.w=255;
	}

    write_imageui(imageC, coord, C);
}
