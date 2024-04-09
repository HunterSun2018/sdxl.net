// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using StableDiffusion.NET;
using StableDiffusion.NET.Helper.Images;
using StableDiffusion.NET.Helper.Images.Colors;

Console.WriteLine("Hello, World!");
string vaePath = "/home/hunter/Projects/cpp/ai/stable-diffusion.cpp/models/sdxl.vae.safetensors";
string modelPath = "/home/hunter/Projects/cpp/ai/stable-diffusion.cpp/models/sd_xl_base_1.0.safetensors";

using StableDiffusionModel sd = new(modelPath, new ModelParameter { VaePath = vaePath });
using StableDiffusionImage image = sd.TextToImage("retail packaging style Nike Shoes. vibrant, enticing, commercial, product-focused, eye-catching, professional, highly detailed",
                                    new StableDiffusionParameter
                                    {
                                        Width = 1024,
                                        Height = 1024,
                                        NegativePrompt = "noisy, blurry, amateurish, sloppy, unattractive",
                                        Seed = 1712560398,
                                        SampleMethod = Sampler.Euler,
                                        SampleSteps = 20
                                    });

IImage? img = image.ToImage();

byte[] data = ImageConversion.ToPng(img);
File.WriteAllBytes("test.png", data);
