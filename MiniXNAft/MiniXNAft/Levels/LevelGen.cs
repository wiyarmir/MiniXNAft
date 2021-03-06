﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MiniXNAft.Levels.Tiles;

namespace MiniXNAft.Levels {
    public class LevelGen {
        private static readonly Random random = new Random();
        public double[] values;
        private int w, h;

        public LevelGen(int w, int h, int featureSize) {
            this.w = w;
            this.h = h;

            values = new double[w * h];

            for (int y = 0; y < w; y += featureSize) {
                for (int x = 0; x < w; x += featureSize) {
                    setSample(x, y, random.NextDouble() * 2 - 1);
                }
            }

            int stepSize = featureSize;
            double scale = 1.0 / w;
            double scaleMod = 1;
            do {
                int halfStep = stepSize / 2;
                for (int y = 0; y < w; y += stepSize) {
                    for (int x = 0; x < w; x += stepSize) {
                        double a = sample(x, y);
                        double b = sample(x + stepSize, y);
                        double c = sample(x, y + stepSize);
                        double d = sample(x + stepSize, y + stepSize);

                        double e = (a + b + c + d) / 4.0
                                + (random.NextDouble() * 2 - 1) * stepSize * scale;
                        setSample(x + halfStep, y + halfStep, e);
                    }
                }
                for (int y = 0; y < w; y += stepSize) {
                    for (int x = 0; x < w; x += stepSize) {
                        double a = sample(x, y);
                        double b = sample(x + stepSize, y);
                        double c = sample(x, y + stepSize);
                        double d = sample(x + halfStep, y + halfStep);
                        double e = sample(x + halfStep, y - halfStep);
                        double f = sample(x - halfStep, y + halfStep);

                        double H = (a + b + d + e) / 4.0
                                + (random.NextDouble() * 2 - 1) * stepSize * scale
                                * 0.5;
                        double g = (a + c + d + f) / 4.0
                                + (random.NextDouble() * 2 - 1) * stepSize * scale
                                * 0.5;
                        setSample(x + halfStep, y, H);
                        setSample(x, y + halfStep, g);
                    }
                }
                stepSize /= 2;
                scale *= (scaleMod + 0.8);
                scaleMod *= 0.3;
            } while (stepSize > 1);
        }

        private double sample(int x, int y) {
            return values[(x & (w - 1)) + (y & (h - 1)) * w];
        }

        private void setSample(int x, int y, double value) {
            values[(x & (w - 1)) + (y & (h - 1)) * w] = value;
        }

        public static byte[][] createAndValidateTopMap(int w, int h) {
            int attempt = 0;
            do {
                if (++attempt > 100) {
                    throw new Exception("No vea zi tarda bieo");
                }

                byte[][] result = createTopMap(w, h);

                /*
                byte[] map = new byte[w * h];
                byte[] data = new byte[w * h];
                for (int y = 0; y < h; y++) {
                    for (int x = 0; x < w; x++) {
                        int i = x + y * w;
                        map[i] = Tile.grass.id;
                        data[i] = 0;
                    }
                }

                byte[][] result = { map, data };
                 * */


                int[] count = new int[256];

                for (int i = 0; i < w * h; i++) {
                    count[result[0][i] & 0xff]++;
                }
                if (count[Tile.rock.id & 0xff] < 100)
                    continue;
                if (count[Tile.sand.id & 0xff] < 100)
                    continue;
                if (count[Tile.grass.id & 0xff] < 100)
                    continue;
                // if (count[Tile.tree.id & 0xff] < 100)
                //     continue;

                return result;

            } while (true);
        }

        public static byte[][] createAndValidateUndergroundMap(int w, int h,
                int depth) {
            // int attempt = 0;
            do {
                byte[][] result = createUndergroundMap(w, h, depth);

                int[] count = new int[256];

                for (int i = 0; i < w * h; i++) {
                    count[result[0][i] & 0xff]++;
                }
                if (count[Tile.rock.id & 0xff] < 100)
                    continue;
                if (count[Tile.dirt.id & 0xff] < 100)
                    continue;
                if (count[(Tile.ironOre.id & 0xff) + depth - 1] < 20)
                    continue;
                if (depth < 3)
                    if (count[Tile.stairsDown.id & 0xff] < 2)
                        continue;

                return result;

            } while (true);
        }



        private static byte[][] createTopMap(int w, int h) {
            LevelGen mnoise1 = new LevelGen(w, h, 16);
            LevelGen mnoise2 = new LevelGen(w, h, 16);
            LevelGen mnoise3 = new LevelGen(w, h, 16);

            LevelGen noise1 = new LevelGen(w, h, 32);
            LevelGen noise2 = new LevelGen(w, h, 32);

            byte[] map = new byte[w * h];
            byte[] data = new byte[w * h];
            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    int i = x + y * w;

                    double val = Math.Abs(noise1.values[i] - noise2.values[i]) * 3 - 2;
                    double mval = Math.Abs(mnoise1.values[i] - mnoise2.values[i]);
                    mval = Math.Abs(mval - mnoise3.values[i]) * 3 - 2;

                    double xd = x / (w - 1.0) * 2 - 1;
                    double yd = y / (h - 1.0) * 2 - 1;
                    if (xd < 0)
                        xd = -xd;
                    if (yd < 0)
                        yd = -yd;
                    double dist = xd >= yd ? xd : yd;
                    dist = dist * dist * dist * dist;
                    dist = dist * dist * dist * dist;
                    val = val + 1 - dist * 20;

                    if (val < -0.5) {
                        map[i] = Tile.water.id;
                    } else if (val > 0.5 && mval < -1.5) {
                        map[i] = Tile.rock.id;
                    } else {
                        map[i] = Tile.grass.id;
                    }
                }
            }

            for (int i = 0; i < w * h / 2800; i++) {
                int xs = random.Next(w);
                int ys = random.Next(h);
                for (int k = 0; k < 10; k++) {
                    int x = xs + random.Next(21) - 10;
                    int y = ys + random.Next(21) - 10;
                    for (int j = 0; j < 100; j++) {
                        int xo = x + random.Next(5) - random.Next(5);
                        int yo = y + random.Next(5) - random.Next(5);
                        for (int yy = yo - 1; yy <= yo + 1; yy++)
                            for (int xx = xo - 1; xx <= xo + 1; xx++)
                                if (xx >= 0 && yy >= 0 && xx < w && yy < h) {
                                    if (map[xx + yy * w] == Tile.grass.id) {
                                        map[xx + yy * w] = Tile.sand.id;
                                    }
                                }
                    }
                }
            }


            /*
             * for (int i = 0; i < w * h / 2800; i++) { int xs = random.Next(w);
             * int ys = random.Next(h); for (int k = 0; k < 10; k++) { int x = xs
             * + random.Next(21) - 10; int y = ys + random.Next(21) - 10; for
             * (int j = 0; j < 100; j++) { int xo = x + random.Next(5) -
             * random.Next(5); int yo = y + random.Next(5) -
             * random.Next(5); for (int yy = yo - 1; yy <= yo + 1; yy++) for (int
             * xx = xo - 1; xx <= xo + 1; xx++) if (xx >= 0 && yy >= 0 && xx < w &&
             * yy < h) { if (map[xx + yy * w] == Tile.grass.id) { map[xx + yy * w] =
             * Tile.dirt.id; } } } } }
             */

            //Trees

            for (int i = 0; i < w * h / 400; i++) {
                int x = random.Next(w);
                int y = random.Next(h);
                for (int j = 0; j < 200; j++) {
                    int xx = x + random.Next(15) - random.Next(15);
                    int yy = y + random.Next(15) - random.Next(15);
                    if (xx >= 0 && yy >= 0 && xx < w && yy < h) {
                        if (map[xx + yy * w] == Tile.grass.id) {
                            map[xx + yy * w] = Tile.tree.id;
                        }
                    }
                }
            }


            //Flowers
            /*
            for (int i = 0; i < w * h / 400; i++) {
                int x = random.Next(w);
                int y = random.Next(h);
                int col = random.Next(4);
                for (int j = 0; j < 30; j++) {
                    int xx = x + random.Next(5) - random.Next(5);
                    int yy = y + random.Next(5) - random.Next(5);
                    if (xx >= 0 && yy >= 0 && xx < w && yy < h) {
                        if (map[xx + yy * w] == Tile.grass.id) {
                            map[xx + yy * w] = Tile.flower.id;
                            data[xx + yy * w] = (byte)(col + random.Next(4) * 16);
                        }
                    }
                }
            }
            */

            //Cactus
            /*
            for (int i = 0; i < w * h / 100; i++) {
                int xx = random.Next(w);
                int yy = random.Next(h);
                if (xx >= 0 && yy >= 0 && xx < w && yy < h) {
                    if (map[xx + yy * w] == Tile.sand.id) {
                        map[xx + yy * w] = Tile.cactus.id;
                    }
                }
            }
            */

            return new byte[][] { map, data };
        }

        private static byte[][] createUndergroundMap(int w, int h, int depth) {
            LevelGen mnoise1 = new LevelGen(w, h, 16);
            LevelGen mnoise2 = new LevelGen(w, h, 16);
            LevelGen mnoise3 = new LevelGen(w, h, 16);

            LevelGen nnoise1 = new LevelGen(w, h, 16);
            LevelGen nnoise2 = new LevelGen(w, h, 16);
            LevelGen nnoise3 = new LevelGen(w, h, 16);

            LevelGen wnoise1 = new LevelGen(w, h, 16);
            LevelGen wnoise2 = new LevelGen(w, h, 16);
            LevelGen wnoise3 = new LevelGen(w, h, 16);

            LevelGen noise1 = new LevelGen(w, h, 32);
            LevelGen noise2 = new LevelGen(w, h, 32);

            byte[] map = new byte[w * h];
            byte[] data = new byte[w * h];
            for (int y = 0; y < h; y++) {
                for (int x = 0; x < w; x++) {
                    int i = x + y * w;

                    double val = Math.Abs(noise1.values[i] - noise2.values[i]) * 3 - 2;

                    double mval = Math.Abs(mnoise1.values[i] - mnoise2.values[i]);
                    mval = Math.Abs(mval - mnoise3.values[i]) * 3 - 2;

                    double nval = Math.Abs(nnoise1.values[i] - nnoise2.values[i]);
                    nval = Math.Abs(nval - nnoise3.values[i]) * 3 - 2;

                    double wval = Math.Abs(wnoise1.values[i] - wnoise2.values[i]);
                    wval = Math.Abs(nval - wnoise3.values[i]) * 3 - 2;

                    double xd = x / (w - 1.0) * 2 - 1;
                    double yd = y / (h - 1.0) * 2 - 1;
                    if (xd < 0)
                        xd = -xd;
                    if (yd < 0)
                        yd = -yd;
                    double dist = xd >= yd ? xd : yd;
                    dist = dist * dist * dist * dist;
                    dist = dist * dist * dist * dist;
                    val = val + 1 - dist * 20;

                    if (val > -2 && wval < -2.0 + (depth) / 2 * 3) {
                        if (depth > 2)
                            map[i] = Tile.lava.id;
                        else
                            map[i] = Tile.water.id;
                    } else if (val > -2 && (mval < -1.7 || nval < -1.4)) {
                        map[i] = Tile.dirt.id;
                    } else {
                        map[i] = Tile.rock.id;
                    }
                }
            }

            {
                int r = 2;
                for (int i = 0; i < w * h / 400; i++) {
                    int x = random.Next(w);
                    int y = random.Next(h);
                    for (int j = 0; j < 30; j++) {
                        int xx = x + random.Next(5) - random.Next(5);
                        int yy = y + random.Next(5) - random.Next(5);
                        if (xx >= r && yy >= r && xx < w - r && yy < h - r) {
                            if (map[xx + yy * w] == Tile.rock.id) {
                                map[xx + yy * w] = (byte)((Tile.ironOre.id & 0xff)
                                        + depth - 1);
                            }
                        }
                    }
                }
            }



            return new byte[][] { map, data };
        }

        /*
            public static void main(String[] args) {
                int d = 0;
                while (true) {
                    int w = 128;
                    int h = 128;

                    byte[] map = LevelGen.createAndValidateTopMap(w, h)[0];
                    // byte[] map = LevelGen.createAndValidateUndergroundMap(w, h, (d++
                    // % 3) + 1)[0];
                    // byte[] map = LevelGen.createAndValidateSkyMap(w, h)[0];

                    BufferedImage img = new BufferedImage(w, h,
                            BufferedImage.TYPE_INT_RGB);
                    int[] pixels = new int[w * h];
                    for (int y = 0; y < h; y++) {
                        for (int x = 0; x < w; x++) {
                            int i = x + y * w;

                            if (map[i] == Tile.water.id)
                                pixels[i] = 0x000080;
                            if (map[i] == Tile.grass.id)
                                pixels[i] = 0x208020;
                            if (map[i] == Tile.rock.id)
                                pixels[i] = 0xa0a0a0;
                            if (map[i] == Tile.dirt.id)
                                pixels[i] = 0x604040;
                            if (map[i] == Tile.sand.id)
                                pixels[i] = 0xa0a040;
                            if (map[i] == Tile.tree.id)
                                pixels[i] = 0x003000;
                            if (map[i] == Tile.lava.id)
                                pixels[i] = 0xff2020;
                            if (map[i] == Tile.cloud.id)
                                pixels[i] = 0xa0a0a0;
                            if (map[i] == Tile.stairsDown.id)
                                pixels[i] = 0xffffff;
                            if (map[i] == Tile.stairsUp.id)
                                pixels[i] = 0xffffff;
                            if (map[i] == Tile.cloudCactus.id)
                                pixels[i] = 0xff00ff;
                        }
                    }
                    img.setRGB(0, 0, w, h, pixels, 0, w);
                    JOptionPane.showMessageDialog(
                            null,
                            null,
                            "Another",
                            JOptionPane.YES_NO_OPTION,
                            new ImageIcon(img.getScaledInstance(w * 4, h * 4,
                                    Image.SCALE_AREA_AVERAGING)));
                }
            }
         */
    }
}
